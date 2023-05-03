import React from 'react';
import { Table } from 'antd';

const columns = [
    {
        title: 'Время',
        dataIndex: 'time',
        key: 'time',
    },
    {
        title: 'Понедельник',
        dataIndex: 'monday',
        key: 'monday',
        render: (text, record) =>
            <a>
                {
                    'monday' in record && record.monday.lessons.map(e => {
                            console.log(e.subjectName)
                            return <>{e.subjectName}</>
                        }
                    ).join()
                }
            </a>,
    },
    {
        title: 'Вторник',
        dataIndex: 'tuesday',
        key: 'tuesday',
    },
    {
        title: 'Среда',
        dataIndex: 'wednesday',
        key: 'wednesday',
    },
    {
        title: 'Четверг',
        dataIndex: 'thursday',
        key: 'thursday',
    },
    {
        title: 'Пятница',
        dataIndex: 'friday',
        key: 'friday',
    },
    {
        title: 'Суббота',
        dataIndex: 'saturday',
        key: 'saturday',
    },
];

const timeIntervals = [
    { start: '8:00', end: '9:35' },
    { start: '9:50', end: '11:25' },
    { start: '11:40', end: '13:15' },
    { start: '13:45', end: '15:20' },
    { start: '15:35', end: '17:10' },
    { start: '17:25', end: '19:00' },
    { start: '19:00', end: '20:35' },
];

const LessonString = ( lesson) => {
    let string = `${lesson.subjectName}`;
    if ("fullName" in lesson) {
        string += `:${lesson.fullName}`
    }
    if ("groups" in lesson && lesson.groups != '') {
        string += `:${lesson.groups.join(", ")}`
    }
    if ("classroomNumber" in lesson) {
        string += `:${lesson.classroomNumber}`
    }
    return string;
}

const ScheduleTable = ({ schedule }) => {
    const data = timeIntervals.map((interval) => ({
        key: interval.start,
        time: `${interval.start} - ${interval.end}`,
    }));

    //console.log(schedule)
    const lessons = schedule.lessons;
    for (let i = 1; i < columns.length; i++) {
        const day = columns[i].key;

        for (let j = 0; j < lessons.length; j++) {
            const lesson = lessons[j];
            const index = timeIntervals.findIndex(
                (interval) => interval.start === lesson.startTime && columns[i].title === lesson.dayOfWeek
            );
            if (index !== -1) {
                if (data[index][day] != null) {
                    data[index][day].lessons.push(lesson);
                } else {
                    data[index][day] = {};
                    data[index][day].lessons = [lesson];
                }
            }
        }
    }
    //console.log(data)
    return <Table columns={columns} dataSource={data} pagination={false}/>;
};

export default ScheduleTable;
