import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import {Alert, AlertTitle} from '@material-ui/lab';
import IconButton from '@material-ui/core/IconButton';
import Collapse from '@material-ui/core/Collapse';
import CloseIcon from '@material-ui/icons/Close';

const useStyles = (theme) => ({
    root: {
        width: '100%',
        '& > * + *': {
            marginTop: theme.spacing(2),
        },
    },
});

class RunModelAlert extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
          open: true,
        };
        this.setOpen=this.setOpen.bind(this);
    }

    setOpen(value) {
        this.setState({open: value});
    }

    render() {
        const {classes} = this.props;
        return (
            <div className={classes.root}>
                <Collapse in={this.state.open}>
                    <Alert severity="error"
                        action={
                            <IconButton
                                aria-label="close"
                                color="inherit"
                                size="medium"
                                onClick={() => {
                                    this.setOpen(false);
                                }}
                            >
                                <CloseIcon fontSize="inherit" />
                            </IconButton>
                        }
                    ><AlertTitle>Error</AlertTitle>
                        Please fill out all the required fields.
                    </Alert>
                </Collapse>
            </div>
        );
    }
}

export default withStyles(useStyles)(RunModelAlert);