import { useState } from "react";
import './Chat.css';

export const Chat = ({ messages, chatRoom, sendMessage, logOut }) => {
    const [message, setMessage] = useState("");

    const onSendMessage = () => {
        sendMessage(message);
        setMessage("");
    };

    const handleKeyPress = (e) => {
        if (e.key === "Enter") {
            onSendMessage();
        }
    };

    return (
        <div className="chat-container">
            <h1>{chatRoom}</h1>

            <div className="messages">
                <ul>
                    {messages.map((msg, index) => (
                        <li key={index}>
                            <strong>{msg.userName}:</strong> {msg.message}
                            <span className={`sentiment ${msg.sentiment}`}>
                                ({msg.sentiment})
                            </span>
                        </li>
                    ))}
                </ul>
            </div>

            <div className="input-area">
                <input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onKeyDown={handleKeyPress}
                    placeholder="Enter your message"
                />
                <button onClick={onSendMessage}>Send</button>
            </div>

            <button className="logout-btn" onClick={logOut}>Log out</button>
        </div>
    );
};
