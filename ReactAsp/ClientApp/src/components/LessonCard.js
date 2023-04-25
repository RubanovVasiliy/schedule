import { Card } from 'antd';

const LessonCard = ({ lesson }) => {
    return (
        <Card style={{ backgroundColor: '#F0F8FF', borderColor: '#87CEFA', borderWidth: '2px' }}>
            <p>{lesson.dayOfWeek}, {lesson.startTime} - {lesson.endTime}</p>
            <p>{lesson.subjectName}</p>
            <p>{lesson.fullName}</p>
        </Card>
    );
};

export default LessonCard;