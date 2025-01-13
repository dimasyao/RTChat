import { useState } from "react";
import './WaitingRoom.css';  

export const WaitingRoom = ({ joinChat }) => {
    const [userName, setUserName] = useState("");
    const [chatRoom, setChatRoom] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const submit = async (e) => {
        e.preventDefault();
        if (isSubmitting) return;
        setIsSubmitting(true); 
        try {
            await joinChat(userName, chatRoom); 
        } finally {
            setIsSubmitting(false); 
        }
    };

    return (
        <form onSubmit={submit} className="waiting-room-container">
            <h1>RTChat</h1>
            <div>
                <h2>User name</h2>
                <input
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    type="text"
                    placeholder="Enter your name"
                    required
                />
            </div>
            <div>
                <h2>Chat room</h2>
                <input
                    value={chatRoom}
                    onChange={(e) => setChatRoom(e.target.value)}
                    type="text"
                    placeholder="Enter chat room"
                    required
                />
            </div>
            <div>
                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? "Wait" : "Join"}
                </button>
            </div>
        </form>
    )
}
