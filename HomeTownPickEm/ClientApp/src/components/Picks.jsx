import React from 'react';
import Schedule from './Schedule';

import PicksTable from './picks/PicksTable';

const Picks = () => {
    return (
        <div>
            <Schedule/>
            <PicksTable/>
        </div>
    );
};

export default Picks;
