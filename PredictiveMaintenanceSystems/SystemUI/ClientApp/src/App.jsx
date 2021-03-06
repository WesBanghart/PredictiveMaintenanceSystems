import React, {Component} from 'react';
import Dashboard from './components/Dashboard'
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Link from '@material-ui/core/Link';
import Paper from '@material-ui/core/Paper';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import {makeStyles} from '@material-ui/core/styles';

import './custom.css'
import './components/Workflow.scss'

//Main driver for the UI
export default class App extends Component {
    //Construct all the components
    constructor(props)
    {
        super(props);
        this.state = 
        {
            isLoaded:false,
            user: [],
            models: [],
            dataSources: [],
        }
        this.getModels = this.getModels.bind(this);
        this.getDataSources = this.getDataSources.bind(this);
    }
    static displayName = App.name;

    dashboardLogin = true;

    //The UI is loading so need to pull data from API
    componentDidMount()
    {
        fetch("https://localhost:5001/api/User"
            ).then(function(response) {
            return response.json();
        }).then(jsonData => this.setState({user: jsonData[0]}))
        this.getModels();
        this.getDataSources();
    };

    //Loads the models from the database
    getModels() {
        fetch("https://localhost:5001/api/Model"
        ).then(function(response) {
            return response.json();
        }).then(jsonData => this.setState({models: jsonData}))
    }

    //Loads the data sources from the database
    getDataSources() {
        fetch("https://localhost:5001/api/DataSource"
        ).then(function(response) {
            return response.json();
        }).then(jsonData => this.setState({dataSources: jsonData}))
    }

    //Renders dashboard if the user is logged in
    render() {
        if (!this.dashboardLogin) {
            return (<UserLogin/>);
        } else {
            return (<Dashboard userData={this.state.user} modelData={this.state.models} dataSourceData={this.state.dataSources}/>);
        }
    }
}

//Old user login placeholder
const UserLogin: React.FC = () => {
    const classes = useStyles();
    return (
        <Grid container component="main" className={classes.root}>
            <CssBaseline/>
            <Grid item xs={false} sm={4} md={7} className={classes.image}/>
            <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
                <div className={classes.paper}>
                    <Avatar className={classes.avatar}>
                        <LockOutlinedIcon/>
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Sign in
                    </Typography>
                    <form className={classes.form} noValidate>
                        <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            id="email"
                            label="Email Address"
                            name="email"
                            autoComplete="email"
                            autoFocus
                        />
                        <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                        />
                        <FormControlLabel
                            control={<Checkbox value="remember" color="primary"/>}
                            label="Remember me"
                        />
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            className={classes.submit}>
                            Sign In
                        </Button>
                        <Grid container>
                            <Grid item xs>
                                <Link href="#" variant="body2">
                                    Forgot password?
                                </Link>
                            </Grid>
                            <Grid item>
                                <Link href="#" variant="body2">
                                    {"Don't have an account? Sign Up"}
                                </Link>
                            </Grid>
                        </Grid>
                        <Box mt={5}>
                            <Copyright/>
                        </Box>
                    </form>
                </div>
            </Grid>
        </Grid>);
};

function Copyright() {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © '}
            <Link color="inherit" href="https://google.com/">
                Predictive Maintenance Systems
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}


const useStyles = makeStyles(theme => ({
    root: {
        height: '100vh',
    },
    image: {
        backgroundImage: 'url(https://www.manufacturingglobal.com/sites/default/files/styles/slider_detail/public/topic/image/GettyImages-846859964_0.jpg?itok=x2ecmFaV)',
        backgroundRepeat: 'no-repeat',
        backgroundColor:
            theme.palette.type === 'dark' ? theme.palette.grey[900] : theme.palette.grey[50],
        backgroundSize: 'cover',
        backgroundPosition: 'center',
    },
    paper: {
        margin: theme.spacing(8, 4),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(1),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));