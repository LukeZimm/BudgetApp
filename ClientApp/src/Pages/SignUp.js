import React, { useEffect, useState } from 'react';
import {usePlaidLink} from 'react-plaid-link';
import { useMsal } from "@azure/msal-react";
import { useIsAuthenticated } from '@azure/msal-react';
import { loginRequest } from "../authConfig";


const SignUp = () => {
  const { accounts } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const [linkToken, setLinkToken] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!isAuthenticated) return;
    // check if user has account in database
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
        if (data.accountExists) {
          // redirect to home page
          window.location.href = "/site";
        } else {
          // continue with sign up
          fetch('//localhost:44347/api/plaid/link_token', {
            method: 'post',
          })
          .then(response => response.json())
          .then(data => {
            console.info(data);
            setLinkToken(data.value);
            setLoading(false);
          })
        }
      });
    }
  },[]);

  const { open, ready } = usePlaidLink({
    token: linkToken,
    onSuccess: (public_token, metadata) => {
      fetch('//localhost:44347/api/plaid/access_token', {
      method: 'post',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        publicToken: public_token,
        email: accounts[0].username,
      }),
    })
    .then(response => response.json())
    .then(data => window.location.href = "/site")
    },
  });

  return <>
    <h1>Sign Up</h1>
    {isAuthenticated ? <></> : <p><strong>Please Sign In</strong></p>}
    {loading ? <p>Loading...</p> : <>
      <h2>Connect to Plaid</h2>
      <p>Please connect your bank account with Plaid to continue.</p>
      <button onClick={() => open()} disabled={!ready}>Connect</button>
    </>}
  </>;
};

export default SignUp;