import React, { useState, useEffect } from 'react';
import { Select, Spin } from 'antd';
import axios from 'axios';
import LessonCard from "./LessonCard";

const { Option } = Select;

const ClassroomSelector = () => {
    const [classrooms, setClassrooms] = useState([]);
    const [loading, setLoading] = useState(false);
    const [classroomInfo, setClassroomInfo] = useState(null);

    useEffect(() => {
        setLoading(true);
        axios.get('/classroom')
            .then(res => {
                setClassrooms(res.data);
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
                {classrooms.map(classroom => (
                    <Option key={classroom.id} value={classroom.id}>
                        {classroom.classroomNumber}
                    </Option>
                ))}
            </Select>
            {loading && <Spin />}
            {classroomInfo && (
                <div>
                    <h2>{classroomInfo.classroomNumber}</h2>
                    <h3>Расписание занятий:</h3>
                    <ul>
                        {classroomInfo.lessons.map(lesson => (
                            <li key={lesson.id}>
                                <LessonCard lesson={lesson}/>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default ClassroomSelector;
