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

export default function ContainedButtons() {
    const classes = useStyles();

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