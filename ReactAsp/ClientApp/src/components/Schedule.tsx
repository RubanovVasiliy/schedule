import { Component } from 'react';
import RadioButtonSelector from "./RadioButtonSelector";

export class Schedule extends Component {
    static displayName = Schedule.name;

    render() {
        return (
            <div>
                <h1>Расписание кафедры вычислительных систем</h1>
                <p>Для просмотра расписания выберите категорию в которая вас интересует</p>
                <div style={{margin: "10px 0 10px 0"}}>
                    <RadioButtonSelector/>
                </div>
            </div>
        );
    }
}
