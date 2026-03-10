import React from 'react'

type ErrorToastProps = {
    title: string
    message: string
    closeToast?: () => void
}

export const ErrorToast: React.FC<ErrorToastProps> = ({ title, message, closeToast }) => {
    return (
        <div>
            <strong>{title}</strong>
            <p>{message}</p>
            <button onClick={closeToast}>OK</button>
        </div>
    )
}