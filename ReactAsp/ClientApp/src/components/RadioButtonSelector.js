import React, { useState } from 'react';
import { Radio } from 'antd';
import EntitySelector from "./EntitySelector";

const RadioButtonSelector = ({ onSelect }) => {
    const [selectedOption, setSelectedOption] = useState('A');

    const handleOptionChange = e => {
        const value = e.target.value;
        setSelectedOption(value);
        onSelect(value);
    };

    return (
        <div>
            <Radio.Group onChange={handleOptionChange} value={selectedOption}>
                <Radio.Button value="A">класс</Radio.Button>
                <Radio.Button value="B">группа</Radio.Button>
                <Radio.Button value="C">преподователь</Radio.Button>
            </Radio.Group>
            {selectedOption === 'A' &&
                <EntitySelector entityName={"класс"} entityDisplayKey={'classroomNumber'} entityIdKey={'id'} entityEndpoint={'classrooms'}/>

            }
            {selectedOption === 'B' &&
                <EntitySelector entityName={"группа"} entityDisplayKey={'classroomNumber'} entityIdKey={'id'} entityEndpoint={'classrooms'}/>

            }
            {selectedOption === 'C' &&             
                <EntitySelector entityName={"преподователь"} entityDisplayKey={'classroomNumber'} entityIdKey={'id'} entityEndpoint={'classrooms'}/>
            }
        </div>
    );
};

export default RadioButtonSelector;
