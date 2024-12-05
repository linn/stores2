import React from 'react';
import { NavigationUI } from '@linn-it/linn-form-components-library';
import { useAuth } from 'react-oidc-context';
import useInitialise from '../hooks/useInitialise';
import config from '../config';

function Navigation() {
    const { isLoading: menuLoading, result: menuData } = useInitialise(
        'https://app.linn.co.uk/intranet/menu-no-auth'
    );
    const { result: notifcationsData } = useInitialise('https://app.linn.co.uk/notifications');
    const auth = useAuth();
    return (
        <NavigationUI
            loading={menuLoading}
            sections={menuData?.sections}
            myStuff={menuData?.myStuff}
            username={auth?.user?.profile?.preferred_username}
            seenNotifications={[]}
            unseenNotifications={notifcationsData?.notifcations}
            markNotificationSeen={() => {}}
            authRoot={config.authorityUri}
        />
    );
}

export default Navigation;
