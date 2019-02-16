import { createBrowserHistory } from 'history';

export default class BaseAPI {

    constructor(accessToken) {
        this.baseAddress = "";
        this.accessToken = accessToken;

        this.GET = "GET";
        this.POST = "POST";
        this.PUT = "PUT";
        this.DELETE = "DELETE";

    }

    
     CreatePayload = (body = {}, method = this.GET,accessToken="")=> {
        switch (method) {
            case this.GET:
                return {
                    method,
                    headers: {
                        "Authorization": `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    },
                }
            case this.POST:
            return {
                    method : method,
                    headers: {
                        "Authorization": `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
            }
            case this.PUT:
                return {
                    method,
                    headers: {
                        "Authorization": `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                }
                case this.DELETE:
                return {
                    method,
                    headers: {
                        "Authorization": `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                }
        }

    }


     ProcessResponse = async (response)=> {
       
        let responseBody = {};

        if (response.ok)
        {
      
            responseBody = await response.json();

            return {
                payload: responseBody,
                statusCode: response.status,
                successful: true,
            }
        }
        else{
            if(response.status===401){
                console.log("Unauthorized");
                createBrowserHistory().push("/auth/login");
                createBrowserHistory().go();
            }
            else if(response.status!==500 && response.status!==502){
               responseBody = await response.json();
            }
            return {
                statusCode: response.status,
                payload: responseBody,
                successful: false,

            }
        }

    }
}