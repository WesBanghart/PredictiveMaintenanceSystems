import React from 'react';
import {withStyles} from '@material-ui/core/styles';
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
import RunModelAlert from "./RunModelAlert";
import SaveModelAlert from "./SaveModelAlert";
import CloudUploadIcon from "@material-ui/icons/CloudUpload";


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

let passedThroughModels = [
    {
        "modelId":"90f4df6d-a844-44fd-ccd7-08d7c6aabda2",
        "modelName":"\"My_Model_1\"",
        "configuration":"\"{}\"",
        "file":null,
        "created":"2020-03-12T10:28:12.401512",
        "lastUpdated":"2020-04-05T16:36:03.0274553",
        "timestamp":"AAAAAAAAE44=",
        "dataSources":null,
        "userId":"8c166ec3-482d-41ca-75aa-08d7c61e9044",
        "user":null,
        "tenantId":"9eae9863-ae02-479e-9d72-08d7c61e4856",
        "tenant":null
    },
    {"modelId":"df218369-2571-400b-ccd8-08d7c6aabda2",
        "modelName":"\"My_Model_2\"",
        "configuration":"\"{JSON STRING}\"",
        "file":null,
        "created":"2020-03-12T10:28:40.463769",
        "lastUpdated":"2020-03-12T10:28:40.463733",
        "timestamp":"AAAAAAAAD6w=",
        "dataSources":null,
        "userId":"8c166ec3-482d-41ca-75aa-08d7c61e9044",
        "user":null,
        "tenantId":"9eae9863-ae02-479e-9d72-08d7c61e4856",
        "tenant":null
    }];

class SimpleModel extends React.Component {
    constructor() {
        super();
        this.state = {
            dataSource: "",
            transformation: "",
            algorithm: "",
            postId: "",
            errorMessage: "",
            showRunModelAlert: false,
            showSaveModelAlert: false,
            saveModelStatus: "",
            selectedModel: "",
            savedModels: passedThroughModels,
        };
        this.setDataSource = this.setDataSource.bind(this);
        this.setTransformation = this.setTransformation.bind(this);
        this.setAlgorithm = this.setAlgorithm.bind(this);
        this.verifyMenuSelections = this.verifyMenuSelections.bind(this);
        this.postData = this.postData.bind(this);
        this.runModelVerification = this.runModelVerification.bind(this);
        this.saveModel = this.saveModel.bind(this);
        this.setModel = this.setModel.bind(this);
        this.localModelSave = this.localModelSave.bind(this);
        this.putData = this.putData.bind(this);
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

    setModel(event) {
        this.setState({selectedModel: event.target.value});
    }

    verifyMenuSelections() {
        if (!this.state.dataSource || this.state.dataSource.length === 0) {
            return false;
        } else if (!this.state.transformation || this.state.transformation.length === 0) {
            return false;
        } else return !(!this.state.algorithm || this.state.algorithm.length === 0);
    }

    postData() {
        try {
            const requestOptions = {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(this.state),
            };
            fetch("https://localhost:5001/api/", requestOptions).then(async response => {
                const data = await response.json();
                if(!response.ok) {
                    const error = (data && data.message) || response.status;
                    return Promise.reject(error);
                }
                this.setState({postId: data.id});
            }).catch(error => {
                this.setState({errorMessage: error});
                console.log("There was an error!", error)
            });
            return true;
        } catch {
            return false;
        }
    }

    putData() {
        try {
            const requestOptions = {
                method: "PUT",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(this.state),
            };
            fetch("https://localhost:5001/api/", requestOptions).then(async response => {
                const data = await response.json();
                if(!response.ok) {
                    const error = (data && data.message) || response.status;
                    return Promise.reject(error);
                }
                this.setState({postId: data.id});
            }).catch(error => {
                this.setState({errorMessage: error});
                console.log("There was an error!", error)
            });
            return true;
        } catch {
            return false;
        }
    }

    runModelVerification() {
        if(this.verifyMenuSelections()) {
            this.postData();
        }
        else {
            this.setState({showRunModelAlert: true});
        }
    }

    saveModel() {
        if(this.postData()) {
            this.setState({showSaveModelAlert: true, saveModelStatus: "success"});
        }
        this.setState({showSaveModelAlert: true, saveModelStatus: "error"});
    }

    localModelSave() {

    }

    render()
    {
        const {classes} = this.props;
        let savedModelsMenuTemplate = this.state.savedModels.map(v => (
            <MenuItem value={v.modelId}>{v.modelName}</MenuItem>
        ));
        return (
            <div>
                {this.state.showRunModelAlert ?
                    <RunModelAlert /> :
                    null
                }
                {this.state.showSaveModelAlert ?
                    <SaveModelAlert status={this.state.saveModelStatus} /> :
                    null
                }
            <div>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    startIcon={<CloudUploadIcon />}
                    onClick={() => this.saveModel()}
                >
                    Persistent Save
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    startIcon={<SaveIcon />}
                    onClick={() => this.saveModel()}
                >
                    Session Save
                </Button>
                <ColorButton
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    startIcon={<DirectionsRunIcon />}
                    onClick={() => this.runModelVerification()}
                >
                    Save & Run
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
                <FormControl className={classes.formControl}>
                    <InputLabel id="model-selection">Model</InputLabel>
                    <Select
                        labelId="model-selection"
                        id="model-selection"
                        value={this.state.selectedModel}
                        onChange={this.setModel}
                    >
                        <MenuItem value="">
                            <em>None</em>
                        </MenuItem>
                        {savedModelsMenuTemplate}
                    </Select>
                    <FormHelperText>Optional</FormHelperText>
                </FormControl>
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
                        <MenuItem value="Source1">Source 1</MenuItem>
                        <MenuItem value="Source2">Source 2</MenuItem>
                        <MenuItem value="Source3">Source 3</MenuItem>
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
                        <MenuItem value="Transformation1">Transformation 1</MenuItem>
                        <MenuItem value="Transformation2">Transformation 2</MenuItem>
                        <MenuItem value="Transformation3">Transformation 3</MenuItem>
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
                        <MenuItem value="Algorithm1">Algorithm 1</MenuItem>
                        <MenuItem value="Algorithm2">Algorithm 2</MenuItem>
                        <MenuItem value="Algorithm3">Algorithm 3</MenuItem>
                    </Select>
                    <FormHelperText>Required</FormHelperText>
                </FormControl>
            </div>
            </div>
            </div>
        );
    }
}

export default withStyles(styles)(SimpleModel);