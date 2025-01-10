import {
  IconAdjustments,
  IconAdjustmentsCog,
  IconCalendarStats,
  IconCar,
  IconFileAnalytics,
  IconFileText,
  IconGauge,
  IconLock,
  IconNotes,
  IconPresentationAnalytics,
  icons,
  IconUserCircle,
  IconUserCog,
} from '@tabler/icons-react';
import { Code, Group, ScrollArea } from '@mantine/core';
import { LinksGroup } from './NavbarLinksGroup/NavbarLinksGroup';
import classes from './NavbarNested.module.css';

const mockdata = [
  { label: 'Dashboard', icon: IconGauge },
  { label: 'Profiel', icon: IconUserCircle, link: '/' },
  {
    label: 'Abonnementen',
    icon: IconNotes,
    initiallyOpened: true,
    links: [
      { label: 'Axanvragen abonnement', link: '/dashboard/abonnement' },
      { label: 'Status abonnementaanvraag', link: '/' },
      { label: 'Abonnement opzeggen', link: '/' },
    ],
  },
  { label: 'Vloot beheren', icon: IconCar, link: '/dashboard/voertuigen' },
  { label: 'Voertuigen', icon: IconCar, link: '/dashboard/huren' },
  { label: 'Controlepaneel', icon: IconAdjustmentsCog, links: [
    { label: 'Verhuuraanvragen beheren', icon: IconFileText, link: '/dashboard/verhuuraanvragen' },
    { label: 'Medewerkers beheren', icon: IconUserCog, link: '/dashboard/medewerkers' },
    ],
  },
  { label: 'Settings', icon: IconAdjustments },
  {
    label: 'Security',
    icon: IconLock,
    links: [
      { label: 'Enable 2FA', link: '/' },
      { label: 'Change password', link: '/' },
      { label: 'Recovery codes', link: '/' },
    ],
  },
];

export function NavbarNested() {
  const links = mockdata.map((item) => <LinksGroup {...item} key={item.label} />);

  return (
    <nav className={classes.navbar}>


      <ScrollArea className={classes.links}>
        <div className={classes.linksInner}>{links}</div>
      </ScrollArea>

      <div className={classes.footer}>
        {/* <UserButton /> */}
      </div>
    </nav>
  );
}