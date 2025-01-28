import {
  IconAdjustmentsCog,
  IconCar,
  IconDoorExit,
  IconFileText,
  IconGauge,
  IconUserCircle,
  IconUserCog,
} from '@tabler/icons-react';
import { Code, Group, ScrollArea } from '@mantine/core';
import { LinksGroup } from './NavbarLinksGroup/NavbarLinksGroup';
import classes from './NavbarNested.module.css';

const menuitems = [
  { label: 'Dashboard', icon: IconGauge },
  {
    label: 'Vloot',
    icon: IconCar,
    initiallyOpened: false,
    links: [
      { label: 'Vloot beheren', link: '/dashboard/voertuigen' }
    ]
  },
  { label: 'Controlepaneel', icon: IconAdjustmentsCog, links: [
    { label: 'Medewerkers beheren', icon: IconUserCog, link: '/dashboard/controlepaneel' },
    { label: 'Verhuuraanvragen beheren', icon: IconFileText, link: '/dashboard/controlepaneel/verhuuraanvragen' },
    { label: 'Verhuuraanvragen afhandelen', icon: IconFileText, link: '/dashboard/controlepaneel/afhandelen' }
    ]
  },
  { label: 'Uitloggen', icon: IconDoorExit, links: [
    { label: 'Log uit', link: '/loguit' },
    ]
  }
];

export function NavbarMedewerkers() {
  const links = menuitems.map((item) => <LinksGroup {...item} key={item.label} />);

  return (
    <nav aria-label="Navigatie menu" className={classes.navbar}>


      <ScrollArea className={classes.links}>
        <div className={classes.linksInner}>{links}</div>
      </ScrollArea>
    </nav>
  );
}