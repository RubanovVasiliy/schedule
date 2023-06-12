import {Button} from '@mui/material'

function getPrefixByDay(day) {
    switch (day) {
        case 'Понедельник':
            return 'MO'
        case 'Вторник':
            return 'TU'
        case 'Среда':
            return 'WE'
        case 'Четверг':
            return 'TH'
        case 'Пятница':
            return 'FR'
        case 'Суббота':
            return 'SA'
        default:
            return ''
    }
}

function getFirstFebruaryDate(dayOfWeek, weekType) {
    const febDate = new Date(new Date().getFullYear(), 1, 1);

    const offset = ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'].indexOf(dayOfWeek.toLowerCase());

    febDate.setDate(1 + offset - febDate.getDay());

    if (febDate.getMonth() < 1) febDate.setDate(febDate.getDate() + 7)

    if (weekType === 0) febDate.setDate(febDate.getDate() + 7)

    return febDate.getDate();
}

function formatDate(date){
    let year = date.getFullYear();
    let month = ('0' + (date.getMonth() + 1)).slice(-2); 
    let day = ('0' + date.getDate()).slice(-2); 
    let hours = ('0' + date.getHours()).slice(-2); 
    let minutes = ('0' + date.getMinutes()).slice(-2); 
    let seconds = ('0' + date.getSeconds()).slice(-2);

    return `${year}${month}${day}T${hours}${minutes}${seconds}`
}

function formatNumberTwoDigit(number) {
    return ('0' + number).slice(-2);
}
function generateICS(schedule) {
    let ics = `BEGIN:VCALENDAR\n`
    ics += `VERSION:2.0\n`
    ics += `PRODID:-//Schedule//EN\n`
    ics += `CALSCALE:GREGORIAN\n`
    ics += `METHOD:PUBLISH\n`;

    ics += `BEGIN:VTIMEZONE\n`
    ics += `TZID:Asia/Novosibirsk\n`
    ics += `BEGIN:STANDARD\n`
    ics += `TZOFFSETFROM:+0700\n`
    ics += `TZOFFSETTO:+0700\n`
    ics += `TZNAME:+07\n`
    ics += `DTSTART:19700101T000000\n`
    ics += `END:STANDARD\n`
    ics += `END:VTIMEZONE\n`

    for (let lesson of schedule.lessons) {
        let uid = `${schedule.id}_${lesson.id}`;

        let lessonStr = `BEGIN:VEVENT\n`

        if (lesson.classroomNumber) {
            lessonStr += `LOCATION:Аудитория: ${lesson.classroomNumber}\n`
        } else {
            lessonStr += `LOCATION:\n`
        }

        if (lesson.groups) {
            lessonStr += `DESCRIPTION:Группы: ${lesson.groups.join(", ")}\n`
        } else {
            lessonStr += `DESCRIPTION:\n`
        }

        lessonStr += `SUMMARY:${lesson.subjectName}\n`

        let interval = lesson.weekType === 3 ? '1' : '2';
        let dayPrefix = getPrefixByDay(lesson.dayOfWeek)

        let endTimeTokens = lesson.endTime.split(':')
        let untilDate = formatDate(
            new Date(`2023-05-31T${formatNumberTwoDigit(endTimeTokens[0])}:${endTimeTokens[1]}:00`)
        )

        lessonStr += `RRULE:FREQ=WEEKLY;UNTIL=${untilDate};INTERVAL=${interval};BYDAY=${dayPrefix};WKST=MO\n`
        let startTimeTokens = lesson.startTime.split(':')

        let februaryDate = getFirstFebruaryDate(lesson.dayOfWeek, lesson.weekType);

        let startDate = formatDate(new Date(`2023-02-${formatNumberTwoDigit(februaryDate)}T${formatNumberTwoDigit(startTimeTokens[0])}:${startTimeTokens[1]}:00`));
        let endDate = formatDate(new Date(`2023-02-${formatNumberTwoDigit(februaryDate)}T${formatNumberTwoDigit(endTimeTokens[0])}:${endTimeTokens[1]}:00`));
        lessonStr += `DTSTART;TZID=Asia/Novosibirsk:${startDate}\n`
        lessonStr += `DTEND;TZID=Asia/Novosibirsk:${endDate}\n`
        lessonStr += `DTSTAMP:${formatDate(new Date())}\n`

        lessonStr += `UID:${uid}\n`
        lessonStr += `END:VEVENT\n`

        ics += lessonStr;
    }

    ics += "END:VCALENDAR";

    return ics;
}

function ICSCreator({schedule}) {
    function downloadICSFile() {
        const icsContent = generateICS(schedule);
        const blob = new Blob([icsContent], {type: "text/calendar;charset=utf-8"});
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");

        link.href = url;
        link.setAttribute("download", `${schedule.groupNumber ? schedule.groupNumber : schedule.fullName}.ics`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    return (
        <Button
            onClick={downloadICSFile}
            variant="outlined"
            size='small'
            style={{color:"#191970", borderColor:'#191970'}}
        >{console.log(schedule)}
            Скачать рассписание .ics
        </Button>
    );
}

export default ICSCreator;
