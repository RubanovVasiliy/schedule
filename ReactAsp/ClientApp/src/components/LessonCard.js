import { Card } from 'antd';

const LessonCard = ({ lesson }) => {
    return (
        <Card style={{ backgroundColor: '#F0F8FF', borderColor: '#87CEFA', borderWidth: '2px' }}>
            <p>{lesson.dayOfWeek}, {lesson.startTime} - {lesson.endTime}</p>
            <p>{lesson.subjectName}</p>
            <p>{lesson.fullName}</p>
            {lesson.weekType == '1' 
                ? 
                <p>Нечетная неделя</p> 
                : 
                lesson.weekType == '0' 
                    ? 
                    <p>Четная неделя</p> 
                    :
                    <></>
            }
        </Card>
    );
};

export default LessonCard;