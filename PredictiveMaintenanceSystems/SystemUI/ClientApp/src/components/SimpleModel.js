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
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import CreateIcon from '@material-ui/icons/Create';
import Input from '@material-ui/core/Input';
import Checkbox from '@material-ui/core/Checkbox';
import ListItemText from '@material-ui/core/ListItemText';


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

const ITEM_HEIGHT = 48;

const ITEM_PADDING_TOP = 8;

const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 250,
        },
    },
};

const tempDataSources = [
    "Source 1",
    "Source 2",
    "Source 3",
];

let passedThroughModels = [
    {
        "modelId": "90f4df6d-a844-44fd-ccd7-08d7c6aabda2",
        "modelName": "\"My_Model_1\"",
        "configuration": "\"{}\"",
        "file": null,
        "created": "2020-03-12T10:28:12.401512",
        "lastUpdated": "2020-04-05T16:36:03.0274553",
        "timestamp": "AAAAAAAAE44=",
        "dataSources": null,
        "userId": "8c166ec3-482d-41ca-75aa-08d7c61e9044",
        "user": null,
        "tenantId": "9eae9863-ae02-479e-9d72-08d7c61e4856",
        "tenant": null
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
    constructor(props) {
        super(props);
        this.state = {
            userData: this.props.userData,
            dataSources: [],
            transformation: "",
            algorithm: "",
            postId: "",
            errorMessage: "",
            showRunModelAlert: false,
            showSaveModelAlert: false,
            saveModelStatus: "",
            selectedModel: "",
            savedModels: passedThroughModels,
            createNewModelPrompt: false,
            newModelNameHolder: "",
        };
        this.setDataSource = this.setDataSource.bind(this);
        this.setTransformation = this.setTransformation.bind(this);
        this.setAlgorithm = this.setAlgorithm.bind(this);
        this.verifyMenuSelections = this.verifyMenuSelections.bind(this);
        this.postData = this.postData.bind(this);
        this.runModelVerification = this.runModelVerification.bind(this);
        this.saveModel = this.saveModel.bind(this);
        this.setModel = this.setModel.bind(this);
        this.appendModelSave = this.appendModelSave.bind(this);
        this.putData = this.putData.bind(this);
        this.createNewModelName = this.createNewModelName.bind(this);
    }

    setDataSource(event) {
        this.setState({dataSources: event.target.value});
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
        if (!this.state.transformation || this.state.transformation.length === 0) {
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
            fetch("https://localhost:5001/api/Model", requestOptions).then(async response => {
                const data = await response.json();
                if (!response.ok) {
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
        if (this.postData()) {
            this.setState({showSaveModelAlert: true, saveModelStatus: "success"});
        }
        this.setState({showSaveModelAlert: true, saveModelStatus: "error"});
    }

    appendModelSave(event) {
        //For when other options are changeable
    }

    createNewModelName() {
        let newModelData = [{
            "modelId": "",
            "modelName": "\"" + this.state.newModelNameHolder + "\"",
            "configuration": "\"{}\"",
            "file": null,
            "created": "2020-03-12T10:28:12.401512",
            "lastUpdated": "2020-04-05T16:36:03.0274553",
            "timestamp": "AAAAAAAAE44=",
            "dataSources": null,
            "userId": this.state.userData["userId"],
            "user": null,
            "tenantId": "9eae9863-ae02-479e-9d72-08d7c61e4856",
            "tenant": null
        }];
        let modelHolder = this.state.savedModels.concat(newModelData);
        this.setState({createNewModelPrompt: false, savedModels: modelHolder});
    }

    render() {
        const {classes} = this.props;
        let savedModelsMenuTemplate = this.state.savedModels.map(v => (
            <MenuItem value={v.modelId}>{v.modelName}</MenuItem>
        ));
        return (
            <div>
                {this.state.showRunModelAlert ?
                    <RunModelAlert/> :
                    null
                }
                {this.state.showSaveModelAlert ?
                    <SaveModelAlert status={this.state.saveModelStatus}/> :
                    null
                }
                <div>
                    <Button variant="contained" color="primary" startIcon={<CreateIcon/>}
                            onClick={() => this.setState({createNewModelPrompt: true})}>
                        Create New Model
                    </Button>
                    <Dialog open={this.state.createNewModelPrompt}
                            onClose={() => this.setState({createNewModelPrompt: false})}
                            aria-labelledby="create-new-model">
                        <DialogTitle id="create-new-model">Create a New Model</DialogTitle>
                        <DialogContent>
                            <DialogContentText>
                                Please enter the name of the model you would like to create.
                            </DialogContentText>
                            <TextField
                                autoFocus
                                margin="dense"
                                id="name"
                                label="Model Name"
                                type="name"
                                onChange={(event) => this.setState({newModelNameHolder: event.target.value})}
                                fullWidth
                            />
                        </DialogContent>
                        <DialogActions>
                            <Button onClick={() => this.setState({createNewModelPrompt: false})} color="primary">
                                Cancel
                            </Button>
                            <Button onClick={this.createNewModelName} color="primary">
                                Create
                            </Button>
                        </DialogActions>
                    </Dialog>
                    <Button
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        startIcon={<CloudUploadIcon/>}
                        onClick={() => this.saveModel()}
                    >
                        Persistent Save
                    </Button>
                    <Button
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        startIcon={<SaveIcon/>}
                        onClick={() => this.saveModel()}
                    >
                        Save Current Model
                    </Button>
                    <ColorButton
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        startIcon={<DirectionsRunIcon/>}
                        onClick={() => this.runModelVerification()}
                    >
                        Save & Run
                    </ColorButton>
                    <Button
                        variant="contained"
                        color="secondary"
                        className={classes.button}
                        startIcon={<DeleteIcon/>}
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
                        <FormControl className={classes.formControl}>
                            <InputLabel id="data-sources-selection">Data Sources</InputLabel>
                            <Select
                                labelId="data-sources-selection-label"
                                id="data-sources-selection"
                                multiple
                                value={this.state.dataSources}
                                onChange={this.setDataSource}
                                input={<Input/>}
                                renderValue={(selected) => selected.join(', ')}
                                MenuProps={MenuProps}
                            >
                                {tempDataSources.map((source) => (
                                    <MenuItem key={source} value={source}>
                                        <Checkbox checked={this.state.dataSources.indexOf(source) > -1}/>
                                        <ListItemText primary={source}/>
                                    </MenuItem>
                                ))}
                            </Select>
                            <FormHelperText>Optional</FormHelperText>
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