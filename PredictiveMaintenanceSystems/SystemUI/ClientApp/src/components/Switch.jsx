import React from 'react';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';

//Creates a switch to toggle the email notifications page for the user
export default function SwitchLabels() {
    const [state, setState] = React.useState({
        checkedA: true,
        checkedB: true,
    });

    const handleChange = (name: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
        setState({...state, [name]: event.target.checked});
    };

    return (
        <FormGroup row>
            <FormControlLabel
                control={
                    <Switch
                        checked={state.checkedB}
                        onChange={handleChange('checkedB')}
                        value="checkedB"s
                        color="primary"
                    />
                }
                label="Daily Email Notification Updates"
            />
        </FormGroup>
    );
}