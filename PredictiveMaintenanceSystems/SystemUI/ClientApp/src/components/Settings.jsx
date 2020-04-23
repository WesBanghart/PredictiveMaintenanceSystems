import React from 'react';
import {withStyles} from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import ContainedButtons from './Button';
import SwitchLabels from "./Switch";
import Typography from "@material-ui/core/Typography";

const useStyles = {
    root: {
        width: '100%',
        overflowX: 'auto',
    },
    table: {
        minWidth: 650,
    },
};

class Settings extends React.Component{
    constructor(props) {
        super(props);
        this.state = {
            userData: this.props.userData,
        };
    }

    render() {
        const {classes} = this.props;
        return (
            <div>
                <Typography color="textPrimary" variant="h4" component="h2">Settings</Typography>
                <br/>
                <Paper className={classes.root}>
                    <h2 style={{margin: 10 + 'px'}}>Company: The Boring Company</h2>
                    <h3 style={{margin: 10 + 'px'}}>Username: {this.props.userData["userName"]}</h3>
                    <body>
                    <div style={{fontSize: 18 + 'px', margin: 20 + 'px'}}>
                        <p>
                            First Name: {this.props.userData["firstName"]}
                            <br/>Last Name: {this.props.userData["lastName"]}
                            <br/>Primary E-mail: {this.props.userData["email"]}
                            <br/>Account Creation: {this.props.userData["created"]}
                            <br/>Last Account Update: {this.props.userData["lastUpdate"]}
                            <br/>User ID: {this.props.userData["userId"]}
                            <br/>Tenant ID: {this.props.userData["tenantId"]}
                        </p>
                        <SwitchLabels/>
                        <ContainedButtons/>
                    </div>
                    </body>
                </Paper>
            </div>
        );
    }
};

export default withStyles(useStyles)(Settings);