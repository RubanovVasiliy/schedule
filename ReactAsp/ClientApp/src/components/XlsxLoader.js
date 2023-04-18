import React  from 'react';
import {Upload,  message} from 'antd';
import { InboxOutlined } from '@ant-design/icons';

const XlsxLoader = () => {
    
    const { Dragger } = Upload;

    const props = {
        name: 'file',
        accept: '.xlsx, application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        multiple: false,
        action: '/upload',
        onChange(info) {
            const { status } = info.file;
            if (status !== 'uploading') {
                console.log(info.file, info.fileList);
            }
            if (status === 'done') {
                message.success(`${info.file.name} file uploaded successfully.`);
            } else if (status === 'error') {
                message.error(`${info.file.name} file upload failed.`);
            }
        },
    };
    return (
        <div>
            <h1>Upload .xlsx files</h1>
            <Dragger {...props}>
                <p className="ant-upload-drag-icon">
                    <InboxOutlined />
                </p>
                <p className="ant-upload-text">Click or drag file to this area to upload</p>
            </Dragger>
        </div>
    );
};

export default XlsxLoader;