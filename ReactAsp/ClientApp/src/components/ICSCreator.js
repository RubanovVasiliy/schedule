import React from "react";

function generateICS(lessons) {
    let icsString = "BEGIN:VCALENDAR\nVERSION:2.0\n";

    lessons.forEach((lesson) => {
        icsString += "BEGIN:VEVENT\n";
        icsString += `DTSTART:${lesson.startTime.replace(":", "")}00\n`;
        icsString += `DTEND:${lesson.endTime.replace(":", "")}00\n`;
        icsString += `RRULE:FREQ=WEEKLY;BYDAY=${lesson.dayOfWeek.slice(0, 2)}\n`;
        icsString += `SUMMARY:${lesson.subjectName}\n`;
        icsString += `LOCATION:${lesson.classroomNumber}\n`;
        icsString += `DESCRIPTION:${lesson.groups.join(", ")}\n`;
        icsString += "END:VEVENT\n";
    });

    icsString += "END:VCALENDAR\n";

    return icsString;
}

function ICSCreator({lessons}) {

    function downloadICSFile() {
        const icsContent = generateICS(lessons);
        const blob = new Blob([icsContent], { type: "text/calendar;charset=utf-8" });
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");

        link.href = url;
        link.setAttribute("download", "schedule.ics");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    return (
        <button onClick={downloadICSFile}>
            Download Schedule as .ics
        </button>
    );
}

export default ICSCreator;
