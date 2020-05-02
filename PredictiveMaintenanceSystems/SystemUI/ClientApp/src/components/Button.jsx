import React from 'react';
import {makeStyles} from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

const useStyles = makeStyles(theme => ({
    button: {
        margin: theme.spacing(1),
    },
    input: {
        display: 'none',
    },
}));

//Defines the buttons for the settings page
export default function ContainedButtons() {
    const classes = useStyles();
    //Render the buttons on the settings page
    return (
        <div>
            <Button variant="contained" className={classes.button}>
                Reset Username
            </Button>
            <Button variant="contained" component="span" className={classes.button}>
                Reset Password
            </Button>
            <Button variant="contained" component="span" className={classes.button}>
                Logout
            </Button>
            <input
                accept="image/*"
                className={classes.input}
                id="contained-button-file"
                multiple
                type="file"
            />
            <label htmlFor="contained-button-file">
            </label>
        </div>
    );
}