export const msalConfig = {
  auth: {
    clientId: "bfeb364e-ef12-43e6-b11f-5cad0af4e396",
    authority: "https://login.microsoftonline.com/f646942f-f1a7-458d-9e07-aae86a5fab00", // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
    redirectUri: document.getElementById('root').baseURI,
  },
  cache: {
    cacheLocation: "sessionStorage", // This configures where your cache will be stored
    storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  }
};

export const loginRequest = {
  scopes: ["User.Read"]
 };