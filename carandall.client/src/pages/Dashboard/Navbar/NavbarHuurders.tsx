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

const mockdata = [
  { label: 'Dashboard', icon: IconGauge },
  {
    label: 'Profiel',
    icon: IconUserCircle,
    links: [
      { label: 'Gegevens bewerken', link: '/dashboard/profiel' }
    ]
  },
  {
    label: 'Huren',
    icon: IconCar,
    initiallyOpened: false,
    links: [
      { label: 'Voertuigen bekijken', link: '/dashboard/huren' }
    ]
  },
  { label: 'Uitloggen', icon: IconDoorExit, links: [
    { label: 'Log uit', link: '/loguit' },
    ]
  }
];

export function NavbarHuurders() {
  const links = mockdata.map((item) => <LinksGroup {...item} key={item.label} />);

  return (
    <nav aria-label="Navigatie menu" className={classes.navbar}>


      <ScrollArea className={classes.links}>
        <div className={classes.linksInner}>{links}</div>
      </ScrollArea>

      <div className={classes.footer}>
        {/* <UserButton /> */}
      </div>
    </nav>
  );
}