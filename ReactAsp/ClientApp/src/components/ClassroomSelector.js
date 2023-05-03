import React, { useState, useEffect } from 'react';
import { Select, Spin } from 'antd';
import axios from 'axios';
import ScheduleTable from "./ScheduleTable";

const { Option } = Select;

const ClassroomSelector = () => {
    const [classrooms, setClassrooms] = useState([]);
    const [loading, setLoading] = useState(false);
    const [classroomInfo, setClassroomInfo] = useState(null);

    useEffect(() => {
        setLoading(true);
        axios.get('/classroom')
            .then(res => {
                const data = res.data.map(e=>{
                    if(e.classroomNumber==="") 
                        e.classroomNumber="Не указан" 
                    return e;
                });
                setClassrooms(data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });
    }, []);

    const handleClassroomSelect = (value) => {
        setLoading(true);
        axios.get(`/classroom/${value}`)
            .then(res => {
                setClassroomInfo(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });
    };

    return (
        <div>
            <h2>Выберите класс</h2>
            <Select style={{ width: 200 }} loading={loading} onSelect={handleClassroomSelect}>
                {classrooms.sort().map(classroom => (
                    <Option key={classroom.id} value={classroom.id}>
                        {classroom.classroomNumber}
                    </Option>
                ))}
            </Select>
            {loading && <Spin />}
            {classroomInfo && (
                <div>
                    {/*<h2>{classroomInfo.classroomNumber}</h2>
                    <h3>Расписание занятий:</h3>
                    <ul>
                        {classroomInfo.lessons.map(lesson => (
                            <li key={lesson.id}>
                                <LessonCard lesson={lesson}/>
                            </li>
                        ))}
                    </ul>*/}
                    <div>
                        <ScheduleTable schedule={classroomInfo}/>
                        {/*<MyTable/>*/}
                    </div>
                </div>
            )}
        </div>
    );
};

export default ClassroomSelector;
