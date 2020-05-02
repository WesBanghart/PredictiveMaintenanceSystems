import React from 'react';
import {withStyles} from '@material-ui/core/styles';
import {Alert, AlertTitle} from '@material-ui/lab';
import IconButton from '@material-ui/core/IconButton';
import Collapse from '@material-ui/core/Collapse';
import CloseIcon from '@material-ui/icons/Close';

//Alert styles
const useStyles = (theme) => ({
    root: {
        width: '100%',
        '& > * + *': {
            marginTop: theme.spacing(2),
        },
    },
});

//Shows the confirmation for the run model alert
class RunModelAlert extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            open: true,
            status: this.props.status,
        };
        this.setOpen=this.setOpen.bind(this);
    }

    setOpen(value) {
        this.setState({open: value});
    }

    //Render the message based on the success/error code
    render() {
        const {classes} = this.props;
        if(this.state.status === "success") {
            return (
                <div className={classes.root}>
                    <Collapse in={this.state.open}>
                        <Alert severity="success"
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
                        ><AlertTitle>Success!</AlertTitle>
                            Model saved and run is queued.
                        </Alert>
                    </Collapse>
                </div>
            );
        }
        else if (this.state.status === "error") {
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
}

export default withStyles(useStyles)(RunModelAlert);