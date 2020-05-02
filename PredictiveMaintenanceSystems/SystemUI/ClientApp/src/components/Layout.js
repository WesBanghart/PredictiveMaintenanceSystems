import React, {Component} from 'react';
import {Container} from 'reactstrap';
import Navigation from './Navigation';

//Calls relevant functions for the layout of the graph
export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <Navigation/>
                <Container>
                    {this.props.children}
                </Container>
            </div>
        );
    }
}
