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
import CloudDownloadIcon from '@material-ui/icons/CloudDownload';
import Paper from "@material-ui/core/Paper";
import DeleteModelAlert from "./DeleteModelAlert";


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
            savedModels: this.props.modelData,
            createNewModelPrompt: false,
            newModelNameHolder: "",
            newModelHasBeenCreated: false,
            checkBoxDataSources: [],
            runModelStatus: "",
            modelIndex: 0,
            allDataSources: [],
            dataSourcesDidLoad: false,
            showDeleteModelAlert: false,
            deleteModelStatus: "",
        };
        this.setDataSource = this.setDataSource.bind(this);
        this.setTransformation = this.setTransformation.bind(this);
        this.setAlgorithm = this.setAlgorithm.bind(this);
        this.postData = this.postData.bind(this);
        this.saveModel = this.saveModel.bind(this);
        this.setModel = this.setModel.bind(this);
        this.appendModelSave = this.appendModelSave.bind(this);
        this.putData = this.putData.bind(this);
        this.createNewModelName = this.createNewModelName.bind(this);
        this.runModelStatus = this.runModelStatus.bind(this);
        this.getModelResults = this.getModelResults.bind(this);
        this.deleteModel = this.deleteModel.bind(this);
        this.deleteModelStatus = this.deleteModelStatus.bind(this);
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
        let indexTmp = 0;
        for(let i = 0; i < this.props.modelData.length; ++i) {
            if(this.props.modelData[i]["modelId"] === event.target.value) {
                indexTmp = i;
            }
        }
        this.setState({selectedModel: event.target.value, modelIndex: indexTmp});
    }

    postData(url, bodyData) {
        try {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify(bodyData)
            };
            fetch(url, requestOptions)
                .then(async response => {
                    const data = await response.json();
                    if (!response.ok) {
                        const error = (data && data.message) || response.status;
                        return Promise.reject(error);
                    }
                    this.setState({ postId: data.modelId })
                })
                .catch(error => {
                    this.setState({ errorMessage: error });
                    console.error('There was an error!', error);
                });
            return true;
        } catch {
            return false;
        }
    }

    putData(url, body) {
        try {
            const requestOptions = {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify(body)
            };
            fetch(url, requestOptions)
                .then(async response => {
                    const data = await response.json();
                    if (!response.ok) {
                        const error = (data && data.message) || response.status;
                        return Promise.reject(error);
                    }
                    this.setState({ postId: data.modelId })
                })
                .catch(error => {
                    this.setState({ errorMessage: error });
                    console.error('There was an error!', error);
                });
            return true;
        } catch {
            return false;
        }
    }

    runModelStatus() {
        let runData = {
            "modelName": this.props.modelData[this.state.modelIndex]["modelId"],
            "configuration": "{json}",
            "userId": this.state.userData["userId"]
        };
        if(this.postData("https://localhost:5001/api/Model/saveandrun", runData)) {
            this.setState({showRunModelAlert: true, runModelStatus: "success"});
        }
        else {
            this.setState({showRunModelAlert: true, runModelStatus: "error"});
        }
    }

    saveModel() {
        let saveData = {
            "modelName": this.props.modelData[this.state.modelIndex]["modelId"],
            "configuration": "{json}",
            "userId": this.state.userData["userId"]
        };
        if (this.postData("https://localhost:5001/api/Model/save", saveData)) {
            this.setState({showSaveModelAlert: true, saveModelStatus: "success"});
        }
        else{
            this.setState({showSaveModelAlert: true, saveModelStatus: "error"});
        }
    }

    appendModelSave(event) {
        //For when other options are changeable
    }

    createNewModelName() {
        let newModelData = {
            "modelName": this.state.newModelNameHolder,
            "configuration": "{json}",
            "userId": this.state.userData["userId"],
        };
        if(this.postData("https://localhost:5001/api/Model/save", newModelData)) {
            this.setState({createNewModelPrompt: false});
        } else {
            this.setState({createNewModelPrompt: false});

        }
    }


    getModelResults() {

    }

    deleteModelStatus() {
        if(this.state.selectedModel !== "") {
            if(this.deleteModel("https://localhost:5001/api/Model/" + this.props.modelData[this.state.modelIndex]["modelId"])) {
                this.setState({showDeleteModelAlert: true, deleteModelStatus: "success"});
            } else {
                this.setState({showDeleteModelAlert: true, deleteModelStatus: "error"});
            }
        } else {
            this.setState({showDeleteModelAlert: true, deleteModelStatus: "noModelSelected"});
        }
    }

    deleteModel(url) {
        try {
            const requestOptions = {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
            };
            fetch(url, requestOptions)
                .then(async response => {
                    const data = await response.json();
                    if (!response.ok) {
                        const error = (data && data.message) || response.status;
                        return Promise.reject(error);
                    }
                    this.setState({ postId: data.modelId })
                })
                .catch(error => {
                    this.setState({ errorMessage: error });
                    console.error('There was an error!', error);
                });
            return true;
        } catch {
            return false;
        }
    }

    componentDidMount() {

    }

    render() {
        const {classes} = this.props;
        let savedModelsMenuTemplate, dataSourceTemplate;
        let dataSourcesTmp = [];
        if(this.props.modelData == null) {
            savedModelsMenuTemplate = <MenuItem value={"model_1"}>Model Placeholder</MenuItem>
        }
        else {
            savedModelsMenuTemplate = this.props.modelData.map(v => (
                <MenuItem value={v.modelId}>{v.modelName}</MenuItem>
            ));
        }
        if(this.props.dataSourceData == null) {
            dataSourceTemplate = tempDataSources.map((source) => (
                <MenuItem key={source} value={source}>
                    <Checkbox checked={this.state.dataSources.indexOf(source) > -1}/>
                    <ListItemText primary={source}/>
                </MenuItem>
            ));
        }
        else {
            for(let i = 0; i < this.props.dataSourceData.length; ++i) {
                dataSourcesTmp[i] = this.props.dataSourceData[i]["dataSourceName"];
            }
            dataSourceTemplate = dataSourcesTmp.map((source) => (
                <MenuItem key={source} value={source}>
                    <Checkbox checked={this.state.dataSources.indexOf(source) > -1}/>
                    <ListItemText primary={source}/>
                </MenuItem>
            ));
        }
        return (
            <div>
                {this.state.showRunModelAlert ?
                    <RunModelAlert status={this.state.runModelStatus}/> :
                    null
                }
                {this.state.showSaveModelAlert ?
                    <SaveModelAlert status={this.state.saveModelStatus}/> :
                    null
                }
                {this.state.showDeleteModelAlert ?
                    <DeleteModelAlert status={this.state.deleteModelStatus} /> :
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
                        onClick={() => this.runModelStatus()}
                    >
                        Save & Run
                    </ColorButton>
                    <Button
                        variant="contained"
                        color="secondary"
                        className={classes.button}
                        startIcon={<DeleteIcon/>}
                        onClick={() => this.deleteModelStatus()}
                    >
                        Delete
                    </Button>
                    <ColorButton
                        variant="contained"
                        color="secondary"
                        className={classes.button}
                        startIcon={<CloudDownloadIcon/>}
                        onClick={() => this.getModelResults()}
                    >
                        Get Model Results
                    </ColorButton>
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
                                {dataSourceTemplate}
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
                    {this.state.selectedModel !== "" ?
                        <Paper className={classes.root}>
                            <h3 style={{margin: 10 + 'px'}}>Model Name: {this.props.modelData[this.state.modelIndex]["modelName"]}</h3>
                            <body>
                            <div style={{fontSize: 18 + 'px', margin: 20 + 'px'}}>
                                <p>
                                    Model ID: {this.props.modelData[this.state.modelIndex]["modelId"]}
                                    <br/>Model Configuration: {this.props.modelData[this.state.modelIndex]["configuration"]}
                                    <br/>File: {this.props.modelData[this.state.modelIndex]["file"]}
                                    <br/>Last Updated: {this.props.modelData[this.state.modelIndex]["lastUpdated"]}
                                    <br/>User ID: {this.props.modelData[this.state.modelIndex]["userId"]}
                                </p>
                            </div>
                            </body>
                        </Paper>:
                        null
                    }
            </div>
            </div>
        );
    }
}

export default withStyles(styles)(SimpleModel);