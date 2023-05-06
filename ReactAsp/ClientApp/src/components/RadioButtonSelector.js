import React, { useState } from 'react';
import { Radio } from 'antd';
import EntitySelector from "./EntitySelector";

const RadioButtonSelector = () => {
    const [selectedOption, setSelectedOption] = useState('A');

    const handleOptionChange = e => {
        const value = e.target.value;
        setSelectedOption(value);
    };

    return (
        <div>
            <Radio.Group onChange={handleOptionChange} value={selectedOption}>
                <Radio.Button value="A">класс</Radio.Button>
                <Radio.Button value="B">группа</Radio.Button>
                <Radio.Button value="C">преподователь</Radio.Button>
            </Radio.Group>
            <div>
                {selectedOption === 'A' &&
                    <EntitySelector entityName={"класс"} entityDisplayKey={'classroomNumber'} entityIdKey={'id'} entityEndpoint={'classrooms'}/>

                }
                {selectedOption === 'B' &&
                    <EntitySelector entityName={"группу"} entityDisplayKey={'groupNumber'} entityIdKey={'id'} entityEndpoint={'groups'}/>

                }
                {selectedOption === 'C' &&
                    <EntitySelector entityName={"преподователя"} entityDisplayKey={'fullName'} entityIdKey={'id'} entityEndpoint={'teachers'}/>
                }
            </div>
        </div>
    );
};

export default RadioButtonSelector;
