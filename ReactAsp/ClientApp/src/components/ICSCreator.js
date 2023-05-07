import React from "react";
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

function formatDate(date){
    let year = date.getFullYear();
    let month = ('0' + (date.getMonth() + 1)).slice(-2); 
    let day = ('0' + date.getDate()).slice(-2); 
    let hours = ('0' + date.getHours()).slice(-2); 
    let minutes = ('0' + date.getMinutes()).slice(-2); 
    let seconds = ('0' + date.getSeconds()).slice(-2);

    return `${year}${month}${day}T${hours}${minutes}${seconds}`
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
        let uid = `${schedule.id}-${lesson.id}`;

        let startDate = new Date();
        startDate.setDate(startDate.getDate() + (lesson.dayOfWeek.charCodeAt(0) - 1040) % 7 - startDate.getDay());
        startDate.setHours(parseInt(lesson.startTime.substring(0, 2)));
        startDate.setMinutes(parseInt(lesson.startTime.substring(3)));
        let startStr = `${startDate.getUTCFullYear()}${startDate.getUTCMonth() + 1}${startDate.getUTCDate()}T${startDate.getUTCHours()}${startDate.getUTCMinutes()}00Z`;

        let endDate = new Date(startDate.getTime());
        endDate.setHours(parseInt(lesson.endTime.substring(0, 2)));
        endDate.setMinutes(parseInt(lesson.endTime.substring(3)));
        let endStr = `${endDate.getUTCFullYear()}${endDate.getUTCMonth() + 1}${endDate.getUTCDate()}T${endDate.getUTCHours()}${endDate.getUTCMinutes()}00Z`;

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
        console.log(dayPrefix)
        console.log(lesson.dayOfWeek)

        let endTimeTokens = lesson.endTime.split(':')
        let untilDate = formatDate(
            new Date(`2023-05-31T${endTimeTokens[0]}:${endTimeTokens[1]}:00`)
        )
        
        lessonStr += `RRULE:FREQ=WEEKLY;UNTIL=${untilDate};INTERVAL=${interval};BYDAY=${dayPrefix};WKST=MO\n`
        lessonStr += `DTSTART;TZID=Asia/Novosibirsk:   ${startStr}\n`
        lessonStr += `DTEND;TZID=Asia/Novosibirsk:  ${endStr}\n`
        lessonStr += `DTSTAMP:${formatDate(new Date())}\n`


        /*
        
        RRULE:FREQ=WEEKLY;UNTIL=20230531T103500;INTERVAL=2;BYDAY=TU;WKST=MO
        DTSTART;TZID=Asia/Novosibirsk:20230214T090000
        DTEND;TZID=Asia/Novosibirsk:20230214T103500
        DTSTAMP:20230507T133300
        
        */


        lessonStr += `UID:${uid}\n`
        lessonStr += `END:VEVENT\n`

        ics += lessonStr;
    }

    ics += "END:VCALENDAR";

    return ics;
}

function ICSCreator({schedule}) {

    console.log(schedule)
    function downloadICSFile() {
        const icsContent = generateICS(schedule);
        const blob = new Blob([icsContent], {type: "text/calendar;charset=utf-8"});
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");

        link.href = url;
        link.setAttribute("download", "schedule.ics");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    return (
        <Button
            onClick={downloadICSFile}
            variant="outlined">
            Download Schedule as .ics
        </Button>
    );
}

export default ICSCreator;
