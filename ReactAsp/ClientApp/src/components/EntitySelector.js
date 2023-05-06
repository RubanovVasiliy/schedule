import React, { useState, useEffect } from 'react';
import { Select, Spin } from 'antd';
import axios from 'axios';
import ScheduleTable from "./ScheduleTable";

const { Option } = Select;

const EntitySelector = ({ entityName, entityEndpoint, entityIdKey, entityDisplayKey }) => {
    const [entities, setEntities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [entityInfo, setEntityInfo] = useState(null);

    useEffect(() => {
        setLoading(true);
        axios.get(entityEndpoint)
            .then(res => {
                const data = res.data.map(e => {
                    if (e[entityDisplayKey] === "")
                        e[entityDisplayKey] = "Не указано"
                    return e;
                });
                setEntities(data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });
    }, [entityEndpoint]);

    const handleEntitySelect = (value) => {
        setLoading(true);
        axios.get(`${entityEndpoint}/${value}`)
            .then(res => {
                setEntityInfo(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });
    };
    
    return (
        <div>
            <h2>Выберите {entityName}</h2>
            <Select style={{width: 200}} loading={loading} onSelect={handleEntitySelect}>
                {entities.sort().map(entity => (
                    <Option key={entity[entityIdKey]} value={entity[entityIdKey]}>
                        {entity[entityDisplayKey]}
                    </Option>
                ))}
            </Select>
            {loading && <Spin/>}
            {entityInfo && (
                <div>
                    <ScheduleTable schedule={entityInfo}/>
                </div>
            )}
        </div>
    );
};

export default EntitySelector;
