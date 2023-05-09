import React, { Component } from 'react';
import RadioButtonSelector from "./RadioButtonSelector";

export class Schedule extends Component {
    static displayName = Schedule.name;

    render() {
        return (
            <div>
                <h1>Hello, world!</h1>
                <p>Welcome to your new single-page application, built with:</p>
                <RadioButtonSelector/>
            </div>
        );
    }
}
