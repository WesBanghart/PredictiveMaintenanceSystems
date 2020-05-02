import React from 'react'
import {createStyles, makeStyles} from '@material-ui/core/styles'
import List from '@material-ui/core/List'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import Divider from '@material-ui/core/Divider'
import Collapse from '@material-ui/core/Collapse'
import IconExpandLess from '@material-ui/icons/ExpandLess'
import IconExpandMore from '@material-ui/icons/ExpandMore'
import AppMenuItemComponent from './NavigationMenuItemComponent'

export type AppMenuItemProps = AppMenuItemPropsWithoutItems & {
    items?: AppMenuItemProps[]
}

//Checks what is open on the navigation bar
const NavigationMenuItem: React.FC<AppMenuItemProps> = props => {
    const {name, link, Icon, items = []} = props;
    const classes = useStyles();
    const isExpandable = items && items.length > 0;
    const [open, setOpen] = React.useState(false);

    //The user clicks on a navigation bar item
    function handleClick() {
        setOpen(!open)
    }

    const MenuItemRoot = (
        <AppMenuItemComponent className={classes.menuItem} link={link} onClick={handleClick}>
            {/* Display an icon if any */}
            {!!Icon && (
                <ListItemIcon className={classes.menuItemIcon}>
                    <Icon/>
                </ListItemIcon>
            )}
            <ListItemText primary={name} inset={!Icon}/>
            {/* Display the expand menu if the item has children */}
            {isExpandable && !open && <IconExpandMore/>}
            {isExpandable && open && <IconExpandLess/>}
        </AppMenuItemComponent>
    );

    const MenuItemChildren = isExpandable ? (
        <Collapse in={open} timeout="auto" unmountOnExit>
            <Divider/>
            <List component="div" disablePadding>
                {items.map((item, index) => (
                    <NavigationMenuItem {...item} key={index}/>
                ))}
            </List>
        </Collapse>
    ) : null;

    return (
        <>
            {MenuItemRoot}
            {MenuItemChildren}
        </>
    )
};

//The style for the navigation bar selection
const useStyles = makeStyles(theme =>
    createStyles({
        menuItem: {
            '&.active': {
                background: 'rgba(0, 0, 0, 0.5)',
                '& .MuiListItemIcon-root': {
                    color: '#fff',
                },
            },
        },
        menuItemIcon: {
            color: '#3292fd',
        },
    }),
);

export default NavigationMenuItem
