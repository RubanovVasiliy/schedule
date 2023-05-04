import { Chip, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import {Tag} from "antd";

const CustomRender = (record, key) => {
    return (
        <>
            {key in record &&
                record[key].lessons.map((e) => {
                    const color = e.weekType === 0 ? 'geekblue' : e.weekType === 1 ? 'green' : 'default'

                    return (
                        <Tag color={color} key={e.id} style={{maxWidth: "195px", whiteSpace: "normal"}}>
                            <div>{e.subjectName}</div>
                            {e.fullName && <div>{e.fullName}</div>}
                            {e.groups && e.groups.map(g => <div key={g}>{g}</div>)}
                            {e.classroomNumber && <div>{e.classroomNumber}</div>}
                        </Tag>
                    );
                })}
        </>
    );
};

const columns = [
    {
        id: 'time',
        label: 'Время',
        minWidth: 100,
    },
    {
        id: 'monday',
        label: 'Понедельник',
        minWidth: 150,
    },
    {
        label: 'Вторник',
        id: 'tuesday',
        minWidth: 150,
    },
    {
        label: 'Среда',
        id: 'wednesday',
        minWidth: 150,
    },
    {
        label: 'Четверг',
        id: 'thursday',
        minWidth: 150,
    },
    {
        label: 'Пятница',
        id: 'friday',
        minWidth: 150,
    },
    {
        id: 'saturday',
        label: 'Суббота',
        minWidth: 150,
    },
];

const timeIntervals = [
    { start: '8:00' },
    { start: '9:50' },
    { start: '11:40' },
    { start: '13:45' },
    { start: '15:35' },
    { start: '17:25' },
    { start: '19:00' },
];

const ScheduleTable = ({ schedule }) => {
    const data = timeIntervals.map((interval) => ({
        time: interval.start,
    }));

    const lessons = schedule.lessons;

    for (let i = 1; i < columns.length; i++) {
        const day = columns[i].id;

        for (let j = 0; j < lessons.length; j++) {
            const lesson = lessons[j];
            const index = timeIntervals.findIndex(
                (interval) => interval.start === lesson.startTime && columns[i].label === lesson.dayOfWeek
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

    return (
        <TableContainer component={Paper}>
            <Table stickyHeader aria-label="schedule table">
                <TableHead>
                    <TableRow>
                        {columns.map((column) => (
                            <TableCell key={column.id} style={{ minWidth: column.minWidth }}>
                                <Typography variant="subtitle1" color="textSecondary">
                                    {column.label}
                                </Typography>
                            </TableCell>
                        ))}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((row, index) => {
                        return (
                            <TableRow key={index}>
                                <TableCell component="th" scope="row">
                                    <Typography variant="subtitle1" color="textSecondary">
                                        {row.time}
                                    </Typography>
                                </TableCell>
                                <TableCell>{CustomRender(row, 'monday')}</TableCell>
                                <TableCell>{CustomRender(row, 'tuesday')}</TableCell>
                                <TableCell>{CustomRender(row, 'wednesday')}</TableCell>
                                <TableCell>{CustomRender(row, 'thursday')}</TableCell>
                                <TableCell>{CustomRender(row, 'friday')}</TableCell>
                                <TableCell>{CustomRender(row, 'saturday')}</TableCell>
                            </TableRow>
                        );
                    })}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default ScheduleTable;
