import React from 'react';
import clsx from 'clsx';
import Navigation from './Navigation';
import Desktop from './Desktop';
import {withStyles} from '@material-ui/core/styles';
import CssBaseline from '@material-ui/core/CssBaseline';
import Drawer from '@material-ui/core/Drawer';
import Container from '@material-ui/core/Container';
import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Settings from './Settings';
import Devices from './DataSource';
import UserProfile from './UserProfile';
import Workflow from './Workflow';
import PropTypes from 'prop-types';
import SimpleModel from "./SimpleModel";

//Define the styles for the dashboard for looks
const styles = theme => ({
    root: {
        display: 'flex'
    },
    drawerPaper: {
        position: 'relative',
        whiteSpace: 'nowrap',
        width: 270,
        paddingTop: theme.spacing(4),
        paddingBottom: theme.spacing(4),
        background: '#585858',
        color: '#fff'
    },
    content: {
        flexGrow: 1,
        height: '100vh',
        overflow: 'auto'
    },
    container: {
        paddingTop: theme.spacing(4),
        paddingBottom: theme.spacing(4)
    }
});

//Main dashboard for the application
class Dashboard extends React.Component {
    //Create properties to hold new information from the API
    constructor(props) {
        super(props);
        this.state = {
            loadedUserData: false,
            loadedModelData: false,
            data: [],
            models: [],
        };
    }

    //Check if the user data is loaded
    render() {
        const {classes} = this.props;
        if(this.props.userData && !this.state.loadedUserData) {
            try {
                this.setState({data: this.props.userData});
            } catch(error) {
                console.log(error);
            }
        }
        //Pass all the information from the API to the various components
        return (
            <BrowserRouter>
                <div className={clsx('App', classes.root)}>
                    <CssBaseline/>
                    <Drawer
                        variant="permanent"
                        classes={{
                            paper: classes.drawerPaper,
                        }}
                    >
                        <UserProfile userData = {this.state.data}/>
                        <Navigation/>
                    </Drawer>
                    <main className={classes.content}>
                        <Container maxWidth="lg" className={classes.container}>
                            <Switch>
                                <Route path="/" exact component={Desktop}/>
                                <Route path="/data_sources" render={(props) => <Devices {...props} dataSourceData={this.props.dataSourceData} userData={this.props.userData}/>} />
                                <Route path="/simple_model" render={(props) => <SimpleModel {...props} userData={this.props.userData} dataSourceData={this.props.dataSourceData} modelData={this.props.modelData}/>} />
                                <Route path="/workflow" component={Workflow}/>
                                <Route path="/settings" render={(props) => <Settings {...props} userData={this.props.userData}/>}/>
                            </Switch>

                        </Container>
                    </main>
                </div>
            </BrowserRouter>
        );
    }
};

Dashboard.propTypes = {
    classes: PropTypes.object.isRequired,
};


export default withStyles(styles)(Dashboard);