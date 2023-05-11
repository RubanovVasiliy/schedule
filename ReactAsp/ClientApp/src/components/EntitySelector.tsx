import { useState, useEffect } from 'react';
import { Select, Spin } from 'antd';
import axios from 'axios';
import ScheduleTable from "./ScheduleTable";
import ICSCreator from "./ICSCreator";

const { Option } = Select;

const EntitySelector = ({ entityName, entityEndpoint, entityIdKey, entityDisplayKey }) => {
    const [entities, setEntities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [entityInfo, setEntityInfo] = useState(null);
    
    
    function compare(a, b) {
        if (a[entityDisplayKey] < b[entityDisplayKey]) {
            return -1;
        }
        if (a[entityDisplayKey] > b[entityDisplayKey]) {
            return 1;
        }
        return 0;
    }

    useEffect(() => {
        setLoading(true);
        axios.get(entityEndpoint)
            .then(res => {
                const data = res.data;
                data.sort(compare).map(e => {
                    if (e[entityDisplayKey] === "")
                        e[entityDisplayKey] = "Не указано"
                    return e;
                })
                setEntities(data)
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
            <h2 style={{margin:'10px 0 5px 0'}}>Выберите {entityName}</h2>
            <Select style={{width: 300, margin:'5px 0 5px 0'}} loading={loading} onSelect={handleEntitySelect}>
                {entities.sort().map(entity => (
                    <Option key={entity[entityIdKey]} value={entity[entityIdKey]}>
                        {entity[entityDisplayKey]}
                    </Option>
                ))}
            </Select>
            {loading &&
                <div>
                    <Spin size="large"/>
                </div>
            }
            {entityInfo && (
                <div>
                    {entityEndpoint !== 'classrooms' && <ICSCreator schedule={entityInfo}/>}
                    <div style={{margin:'10px 0 50px 0'}}>
                        <ScheduleTable schedule={entityInfo}/>
                    </div>
                </div>
            )}
        </div>
    );
};

export default EntitySelector;
