import {styled, Typography} from '@mui/material';
import { Container, Row, Col } from 'reactstrap';

function Footer() {

    return (
        <footer style={{
            padding: '12px',
            boxShadow: '0 -2px 6px rgba(0, 0, 0, 0.1)',
            backgroundColor: '#191970'
        }}
                className="fixed-bottom">
            <Container>
                <Row>
                    <Col className="text-center text-white">
                        © {new Date().getFullYear()}
                    </Col>
                </Row>
            </Container>
        </footer>
    );
}

export default Footer;

