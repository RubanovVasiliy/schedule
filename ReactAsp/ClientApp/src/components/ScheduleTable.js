import React from 'react';
import { Table ,Tag} from 'antd';

const CustomRender = (record,key) => {
    return <div>
        {
            key in record && record[key].lessons.map(e => {
                    //console.log(e.subjectName)
                    const color = e.weekType === 0 ? 'geekblue' : e.weekType === 1 ? 'green' : 'default'

                    return (
                        <Tag color={color} key={e.id} style={{maxWidth: "195px", whiteSpace: "normal"}}>
                            <div>{e.subjectName}</div>
                            {e.fullName && <div>{e.fullName}</div>}
                            {e.groups && e.groups.map(g => <div key={g}>{g}</div>)}
                            {e.classroomNumber && <div>{e.classroomNumber}</div>}
                        </Tag>
                    );
                }
            )
        }
    </div>
}

const columns = [
    {
        title: 'Время',
        dataIndex: 'time',
        key: 'time',
        width: '100px',
    },
    {
        title: 'Понедельник',
        dataIndex: 'monday',
        key: 'monday',
        width: '15%',
        render: (_, record) =>  CustomRender(record, 'monday')
    },
    {
        title: 'Вторник',
        dataIndex: 'tuesday',
        key: 'tuesday',
        width: '15%',

        render: (_, record) =>  CustomRender(record, 'tuesday')
    },
    {
        title: 'Среда',
        dataIndex: 'wednesday',
        key: 'wednesday',
        width: '15%',

        render: (_, record) =>  CustomRender(record, 'wednesday')

    },
    {
        title: 'Четверг',
        dataIndex: 'thursday',
        key: 'thursday',
        width: '15%',

        render: (_, record) =>  CustomRender(record, 'thursday')

    },
    {
        title: 'Пятница',
        dataIndex: 'friday',
        key: 'friday',
        width: '15%',

        render: (_, record) =>  CustomRender(record, 'friday')

    },
    {
        title: 'Суббота',
        dataIndex: 'saturday',
        key: 'saturday',
        width: '15%',

        render: (_, record) => CustomRender(record, 'saturday')

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
        time: interval.start,
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
    return <Table columns={columns} dataSource={data} scroll={{ x: 1296 }} pagination={false}/>;
};

export default ScheduleTable;
