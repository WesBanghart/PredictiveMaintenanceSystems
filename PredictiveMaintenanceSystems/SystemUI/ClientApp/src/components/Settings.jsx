import React from 'react';
import {makeStyles} from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import ContainedButtons from './Button';
import SwitchLabels from "./Switch";
import Typography from "@material-ui/core/Typography";

const useStyles = makeStyles({
    root: {
        width: '100%',
        overflowX: 'auto',
    },
    table: {
        minWidth: 650,
    },
});

export default function Settings() {
    const classes = useStyles();

    return (
        <div>
            <Typography color="textPrimary" variant="h4" component="h2">Settings</Typography>
            <br/>
            <Paper className={classes.root}>
                <h2 style={{margin: 10 + 'px'}}>Company: The Boring Company</h2>
                <h3 style={{margin: 10 + 'px'}}>Username: jsmith</h3>
                <body>
                <div style={{fontSize: 18 + 'px', margin: 20 + 'px'}}>
                    <p>
                        Primary E-mail: jsmith@theboringcompany.com
                        <br/>Location: Reno, NV
                        <br/> Local Timezone: PST (UTC+05:00)
                        <br/> Team: Process Engineers
                        <br/> Permission Level: Standard
                    </p>
                    <SwitchLabels/>
                    <ContainedButtons/>
                </div>
                </body>
            </Paper>
        </div>
    );
}

