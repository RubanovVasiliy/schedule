import React from 'react';
import { Button } from 'antd';
import { BulbOutlined } from '@ant-design/icons';

const NavBar = ({ darkMode, handleToggleDarkMode }) => {
    return (
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h1 style={{ color: darkMode ? '#fff' : '#000' }}>Upload .xlsx files</h1>
            <Button icon={<BulbOutlined />} onClick={handleToggleDarkMode}>
                {darkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'}
            </Button>
        </div>
    );
};

export default NavBar;