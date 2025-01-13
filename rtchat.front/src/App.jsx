import { HubConnectionBuilder } from '@microsoft/signalr'
import { WaitingRoom } from './components/WaitingRoom'
import { Chat } from './components/Chat'
import { useState } from "react";
import './App.css'

function App() {
    const [connection, setConnection] = useState(null);
    const [chatRoom, setChatRoom] = useState("");
    const [messages, setMessages] = useState([]);

    const joinChat = async (userName, chatRoom) => {
        var connection = new HubConnectionBuilder()
            .withUrl('https://rtchat-backend.azurewebsites.net/chatHub')
            .withAutomaticReconnect()
            .build();

        connection.on('ReceiveMessage', (userName, message, sentiment) => {
            setMessages(messages => [...messages, { userName, message, sentiment }]);
        });

        try {
            await connection.start();
            await connection.invoke("JoinChat", { userName, chatRoom });

            setConnection(connection);
            setChatRoom(chatRoom);
            console.log(connection)
        }catch (error) {
            console.log(error);
        }
    }

    const sendMessage = async (message) => {
        await connection.invoke("SendMessage", message);
    };


    const logOut = async () => {
        await connection.stop();
        setConnection(null);
        setChatRoom("");
        setMessages([]);
    }

    return (
        <div>
            {connection ?
                <Chat messages={messages}
                    chatRoom={chatRoom}
                    sendMessage={sendMessage}
                    logOut={logOut}
                /> :
                <WaitingRoom
                    joinChat={joinChat} 
                />}
        </div>
    )
}

export default App
