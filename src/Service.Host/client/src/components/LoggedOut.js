import React, { useEffect } from 'react';
import { useAuth } from 'react-oidc-context';
import * as authUtils from '../helpers/authUtils';

const LoggedOut = () => {
    const auth = useAuth();
    useEffect(() => {
        if (auth && auth.signoutRedirect) {
            auth.removeUser();
        }
        sessionStorage.clear();
        localStorage.clear();

        authUtils.signOutEntra();
        // only want to run logout logic once on component mount, not on auth changes
        // so ignore the linter this once
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);
    return <div>You are now logged out.</div>;
};

export default LoggedOut;
