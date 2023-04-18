import React, {Component} from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </Layout>
    );
  }
}
/*


import { Upload, message, Button, ConfigProvider } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import NavBar from "./components/NavBar";

const App = () => {
    const [fileList, setFileList] = useState([]);
    const [darkMode, setDarkMode] = useState(false);

    const handleUpload = () => {
        const formData = new FormData();
        formData.append('file', fileList[0]);
        axios.post('upload', formData)
            .then(res => {
                console.log(res);
            })
            .catch(err => {
                console.error(err);
            });
    };

    const handleFileChange = ({ fileList }) => {
        setFileList(fileList);
    };

    const handleToggleDarkMode = () => {
        setDarkMode(!darkMode);
    };

    const props = {
        fileList,
        onChange: handleFileChange,
        showUploadList: {
            showRemoveIcon: false,
        },
        accept: '.xlsx',
        beforeUpload: file => {
            const isXlsx = file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
            if (!isXlsx) {
                message.error('You can only upload .xlsx files!');
            }
            return isXlsx;
        },
    };

    return (
        <ConfigProvider theme={darkMode ? 'dark' : 'light'}>
            <div style={{ background: darkMode ? '#000' : '#fff', padding: '32px', minHeight: '100vh' }}>
                <NavBar darkMode={darkMode} handleToggleDarkMode={handleToggleDarkMode} />
                <Upload {...props} style={{ marginTop: '16px' }}>
                    <Button icon={<UploadOutlined />} style={{ backgroundColor: darkMode ? '#444' : '#fff', color: darkMode ? '#fff' : '#000', borderColor: darkMode ? '#444' : '#fff' }}>Select File</Button>
                </Upload>
                <Button type="primary" onClick={handleUpload} style={{ backgroundColor: darkMode ? '#444' : '#fff', color: darkMode ? '#fff' : '#000', borderColor: darkMode ? '#444' : '#fff', marginTop: '16px' }}>
                    Upload
                </Button>
            </div>
        </ConfigProvider>
    );
};

export default App;
*/
