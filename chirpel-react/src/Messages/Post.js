import React from 'react';
import {useParams} from "react-router";

function Post(){
    let { id } = useParams();

        return<span>{id}</span>
}
export default Post