import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import Button from "@material-ui/core/Button";
import DeleteIcon from "@material-ui/icons/Delete";
import SaveIcon from "@material-ui/icons/Save";
import DirectionsRunIcon from '@material-ui/icons/DirectionsRun';
import green from "@material-ui/core/colors/green";


const ColorButton = withStyles((theme) => ({
    root: {
        color: theme.palette.getContrastText(green[700]),
        backgroundColor: green[700],
        '&:hover': {
            backgroundColor: green[900],
        },
    },
}))(Button);

const styles = theme => ({
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
    button: {
        margin: theme.spacing(1),
    },
});

class SimpleModel extends React.Component {
    constructor() {
        super();
        this.state = {
            dataSource: "",
            transform: "",
            algorithm: "",
        };
        this.setDataSource = this.setDataSource.bind(this);
        this.setTransformation = this.setTransformation.bind(this);
        this.setAlgorithm = this.setAlgorithm.bind(this);
    }

    setDataSource(event) {
        this.setState({dataSource: event.target.value});
    }

    setTransformation(event) {
        this.setState({transformation: event.target.value});
    }

    setAlgorithm(event) {
        this.setState({algorithm: event.target.value});
    }

    render()
    {
        const {classes} = this.props;
        return (
            <div>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    startIcon={<SaveIcon />}
                >
                    Save
                </Button>
                <ColorButton
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    startIcon={<DirectionsRunIcon />}
                >
                    Run
                </ColorButton>
                <Button
                    variant="contained"
                    color="secondary"
                    className={classes.button}
                    startIcon={<DeleteIcon />}
                >
                    Delete
                </Button>
            <div>
                <FormControl required className={classes.formControl}>
                    <InputLabel id="data-source-selection">Data Source</InputLabel>
                    <Select
                        labelId="data-source-selection"
                        id="data-source-selection"
                        value={this.state.dataSource}
                        onChange={this.setDataSource}
                        className={classes.selectEmpty}
                    >
                        <MenuItem value="">
                            <em>Please Select a Data Source</em>
                        </MenuItem>
                        <MenuItem value={"Source1"}>Source 1</MenuItem>
                        <MenuItem value={"Source2"}>Source 2</MenuItem>
                        <MenuItem value={"Source3"}>Source 3</MenuItem>
                    </Select>
                    <FormHelperText>Required</FormHelperText>
                </FormControl>
                <FormControl required className={classes.formControl}>
                    <InputLabel id="transformation-selection">Transformation</InputLabel>
                    <Select
                        labelId="transformation-selection"
                        id="transformation-selection"
                        value={this.state.transformation}
                        onChange={this.setTransformation}
                        className={classes.selectEmpty}
                    >
                        <MenuItem value="">
                            <em>Please Select a Transformation</em>
                        </MenuItem>
                        <MenuItem value={"Transformation1"}>Transformation 1</MenuItem>
                        <MenuItem value={"Transformation2"}>Transformation 2</MenuItem>
                        <MenuItem value={"Transformation3"}>Transformation 3</MenuItem>
                    </Select>
                    <FormHelperText>Required</FormHelperText>
                </FormControl>
                <FormControl required className={classes.formControl}>
                    <InputLabel id="algorithm-selection">Algorithm</InputLabel>
                    <Select
                        labelId="algorithm-selection"
                        id="algorithm-selection"
                        value={this.state.algorithm}
                        onChange={this.setAlgorithm}
                        className={classes.selectEmpty}
                    >
                        <MenuItem value="">
                            <em>Please Select a Algorithm</em>
                        </MenuItem>
                        <MenuItem value={"Algorithm1"}>Algorithm 1</MenuItem>
                        <MenuItem value={"Algorithm2"}>Algorithm 2</MenuItem>
                        <MenuItem value={"Algorithm3"}>Algorithm 3</MenuItem>
                    </Select>
                    <FormHelperText>Required</FormHelperText>
                </FormControl>
            </div>
            </div>
        );
    }
}

export default withStyles(styles)(SimpleModel);