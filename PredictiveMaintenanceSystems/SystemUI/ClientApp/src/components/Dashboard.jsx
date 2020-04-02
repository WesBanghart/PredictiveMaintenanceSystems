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

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loadedData: false,
            data: [],
        };
    }

    render() {
        const {classes} = this.props;
        if(this.props.userData && !this.state.loadedData) {
            try {
                this.setState({data: this.props.userData});
            } catch(error) {
                console.log(error);
            }
        }
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
                                <Route path="/devices" component={Devices}/>
                                <Route path="/simple_model" component={SimpleModel}/>
                                <Route path="/workflow" component={Workflow}/>
                                <Route path="/settings" component={Settings}/>
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