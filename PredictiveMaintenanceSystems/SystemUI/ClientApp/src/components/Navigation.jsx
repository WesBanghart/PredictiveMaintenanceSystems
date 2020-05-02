import React from 'react';
import {createStyles, makeStyles} from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import IconDashboard from '@material-ui/icons/Dashboard';
import SettingsIcon from '@material-ui/icons/Settings';
import AssessmentIcon from '@material-ui/icons/Assessment';
import NavigationMenuItem from './NavigationMenuItem';
import DeviceHubIcon from '@material-ui/icons/DeviceHub';
import InputIcon from '@material-ui/icons/Input';

//Handles the redirection from the navigation bar and URLs
const navigationMenuItems = [
    {
        name: 'Desktop',
        link: '/',
        Icon: IconDashboard,
    },
    {
        name: 'Data Sources',
        link: '/data_sources',
        Icon: DeviceHubIcon,
    },
    {
      name: 'Simple Model',
      link: '/simple_model',
      Icon: InputIcon,
    },
    {
        name: 'Workflow',
        link: '/workflow',
        Icon: AssessmentIcon,
    },
    {
        name: 'Settings',
        link: '/settings',
        Icon: SettingsIcon,
    },
];

//Render each navigation component
const Navigation: React.FC = () => {
    const classes = useStyles();

    return (
        <List component="nav" className={classes.appMenu} disablePadding>
            {navigationMenuItems.map((item, index) => (
                <NavigationMenuItem {...item} key={index}/>
            ))}
        </List>
    )
};


//Styles of the navigation bar
const drawerWidth = 270;
const useStyles = makeStyles(theme =>
    createStyles({
        appMenu: {
            width: '100%',
        },
        navList: {
            width: drawerWidth,
        },
        menuItem: {
            width: drawerWidth,
        },
        menuItemIcon: {
            color: '#97c05c',
        },
    }),
);

export default Navigation