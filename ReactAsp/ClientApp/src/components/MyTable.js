import { Table } from 'antd';
import MyComponent from './MyComponent';

const dataSource = [
    {
        id: 1,
        fieldName: 'Data 1',
    },
    {
        id: 2,
        fieldName: 'Data 2',
    },
];

const columns = [
    {
        title: 'ID',
        dataIndex: 'id',
        key: 'id',
    },
    {
        title: 'Field Name',
        dataIndex: 'fieldName',
        key: 'fieldName',
        render: (text, record) => <>
            <div>
                <MyComponent data={text} />
            </div>
            <div>
                <MyComponent data={text} />
            </div>
        </>,
    },
];

const MyTable = () => {
    return <Table dataSource={dataSource} columns={columns} pagination={false}/>;
};

export default MyTable;
