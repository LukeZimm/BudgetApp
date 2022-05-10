import React, {useState, useEffect} from 'react';
import { useMsal } from "@azure/msal-react";
import { useIsAuthenticated } from '@azure/msal-react';

const Site = () => {
    const isAuthenticated = useIsAuthenticated();
    const { accounts } = useMsal();
    const [balances, setBalances] = useState("");
    const [loading, setLoading] = useState(true);
    
    // TODO: check if user has account in database
    
    if (isAuthenticated) {
        fetch('//localhost:44347/api/auth', {
            method: 'post',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify({email: accounts[0].username}),
        })
        .then(response => response.json())
        .then(data => {
            if (!data.accountExists) {
                // redirect to sign up page
                window.location.href = "/sign-up";
            } else {
                // continue with sign up
                // TODO: 
            }
        });
    }

    // TODO: Fetch balances from api
    useEffect(() => {
        fetch('//localhost:44347/api/plaid/balances', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                email: accounts[0].username,
            }),
        })
        .then(response => response.json())
        .then(data => {
            console.info(data);
            setBalances(data);
            setLoading(false);
        });
    }, []);

    return <>
        <h1>Site</h1>
        { (loading) ? <p>Loading...</p> : 
        <>
            <h2>Accounts</h2>
            <ul>
                {balances.accounts.map(account => <li>{account.name}: {account.balances.current}</li>)}
            </ul>
        </>
        }

    </>;
}

export default Site;