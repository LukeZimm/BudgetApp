import React, { useEffect, useState } from 'react';
import {usePlaidLink} from 'react-plaid-link';
import { useIsAuthenticated } from '@azure/msal-react';

import { useMsal } from "@azure/msal-react";


const Home = () => {
  self.displayName = Home.name;

  const isAuthenticated = useIsAuthenticated();
  const [linkToken, setLinkToken] = useState("");
  const { instance, accounts, inProgress } = useMsal();

  useEffect(() => {
    // TODO: Fetch if user has account in database
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
          // TODO: redirect to account creation page
          window.location.href = "/site";
        } else {
          // TODO: direct to home page
          window.location.href = "/sign-up";
        }
      });
    }
  },[isAuthenticated]);

  useEffect(() => {
    fetch('//localhost:44347/api/plaid/link_token', {
      method: 'post',
    })
    .then(response => response.json())
    .then(data => {
      console.info(data);
      setLinkToken(data.value);
    })
  },[]);

  const { open, ready } = usePlaidLink({
    token: linkToken,
    onSuccess: (public_token, metadata) => {
      fetch('//localhost:44347/api/plaid/access_token?publicToken='+public_token, {
      method: 'post',
    })
    .then(response => response.json())
    .then(data => console.info(data))
    },
  });

  return <>
  {isAuthenticated ? <p>Signed In</p> : <p>Please Sign In</p>}
  <button onClick={()=>open()} disabled={!ready}>Plaid Link</button>
    <div>
      <h1>Hello, world!</h1>
      <p>Welcome to your new single-page application, built with:</p>
      <ul>
        <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
        <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
        <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
      </ul>
      <p>To help you get started, we have also set up:</p>
      <ul>
        <li><strong>Client-side navigation</strong>. For example, click <em>Counter</em> then <em>Back</em> to return here.</li>
        <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
        <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
      </ul>
      <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
    </div>
    </>;
};

export default Home;