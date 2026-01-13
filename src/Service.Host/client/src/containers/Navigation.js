import React from 'react';
import { Navigation as NavigationUI } from '@linn-it/linn-form-components-library';
import { useAuth } from 'react-oidc-context';
import useInitialise from '../hooks/useInitialise';
import config from '../config';
import * as authUtils from '../helpers/authUtils';

function Navigation() {
    const { isLoading: menuLoading, result: menuData } = useInitialise(
        'https://app.linn.co.uk/intranet/menu-no-auth'
    );
    const { result: notifcationsData } = useInitialise('https://app.linn.co.uk/notifications');
    const auth = useAuth();

    // don't render the old sign out link on newer apps
    const myStuffWithSignOutLinkRemoved = {
        ...menuData?.myStuff,
        groups: menuData?.myStuff?.groups.filter(
            group => !group.items.some(item => item.href === '/signout')
        )
    };

    // instead pass a working sign out behaviour for this context
    const handleSignOut = () => {
        authUtils.signOut();
    };

    return (
        <NavigationUI
            handleSignOut={handleSignOut}
            loading={menuLoading}
            sections={menuData?.sections}
            myStuff={myStuffWithSignOutLinkRemoved}
            username={auth?.user?.profile?.preferred_username}
            seenNotifications={[]}
            unseenNotifications={notifcationsData?.notifcations}
            markNotificationSeen={() => {}}
            authRoot={config.cognitoHost}
        />
    );
}

export default Navigation;
