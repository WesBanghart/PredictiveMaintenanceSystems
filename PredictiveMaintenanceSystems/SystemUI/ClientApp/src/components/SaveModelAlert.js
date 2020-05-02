import React from 'react';
import {withStyles} from '@material-ui/core/styles';
import {Alert, AlertTitle} from '@material-ui/lab';
import IconButton from '@material-ui/core/IconButton';
import Collapse from '@material-ui/core/Collapse';
import CloseIcon from '@material-ui/icons/Close';

//Styles for the save model
const useStyles = (theme) => ({
    root: {
        width: '100%',
        '& > * + *': {
            marginTop: theme.spacing(2),
        },
    },
});

//Shows a confirmation message when saving a model
class SaveModelAlert extends React.Component {
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

    //Render the correct message based on success/error
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
                        ><AlertTitle>Success</AlertTitle>
                            Model saved successfully!
                        </Alert>
                    </Collapse>
                </div>
            );
        }
        else if(this.state.status === "error") {
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
                            The model could not be saved.
                        </Alert>
                    </Collapse>
                </div>
            );
        }
    }
}

export default withStyles(useStyles)(SaveModelAlert);